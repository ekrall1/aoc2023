{
  description = "flake to build Aoc2023 with local MathNet.Numerics";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs =
    {
      self,
      nixpkgs,
      flake-utils,
      ...
    }:
    flake-utils.lib.eachDefaultSystem (
      system:
      let
        pkgs = import nixpkgs {
          inherit system;
          config = {
            allowUnfree = true;
          };
        };

        # fetch the nuget package file
        mathnetPkg = pkgs.fetchurl {
          url = "https://www.nuget.org/api/v2/package/MathNet.Numerics/5.0.0";
          sha256 = "sha256-RHJCVM6OxquJF7n5Mbe/oNbucBbkge6ULcbAczOgmVo=";
        };

        lib = nixpkgs.lib;

        dotnet = with pkgs; [
          dotnet-sdk
        ];

        azure-iac-env = with pkgs; [
          pulumi-bin
          pulumiPackages.pulumi-language-go
          go
        ];

        azure-cli = pkgs.azure-cli.withExtensions [
          pkgs.azure-cli-extensions.azure-devops
        ];

        vscode = (
          pkgs.vscode-with-extensions.override {
            vscodeExtensions = with pkgs.vscode-extensions; [
              bbenoist.nix
              jnoortheen.nix-ide
              ms-dotnettools.csdevkit
              ms-dotnettools.csharp
              ms-dotnettools.vscode-dotnet-runtime
              golang.go
            ];
          }
        );

        # derivation that puts the .nupkg into a folder
        localNugetRepo = pkgs.stdenv.mkDerivation {
          pname = "local-nuget-repo";
          version = "1.0";
          src = mathnetPkg;
          unpackPhase = "true";
          buildPhase = ''
            mkdir -p $out
            cp $src $out/MathNet.Numerics.5.0.0.nupkg
          '';
          installPhase = "true";
        };

        # build the project using explicit NuGet.Config to add local source
        aoc2023Build = pkgs.stdenv.mkDerivation {
          pname = "aoc2023";
          version = "0.0.1";
          src = ./.;

          nativeBuildInputs = [
            pkgs.dotnet-sdk
            pkgs.unzip
          ];

          buildPhase = ''
                # Write NuGet.Config to add local package source
                cat > NuGet.Config <<EOF
            <?xml version="1.0" encoding="utf-8"?>
            <configuration>
              <packageSources>
                <add key="local" value="file://${localNugetRepo}" />
                <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
              </packageSources>
            </configuration>
            EOF

                export DOTNET_NUGET_CONFIG_FILE=$PWD/NuGet.Config

                dotnet restore ./Aoc2023/Aoc2023.csproj --configfile NuGet.Config
                dotnet build ./Aoc2023/Aoc2023.csproj --configuration Debug
          '';

          installPhase = ''
            mkdir -p $out
            echo "Contents of bin/Debug/net8.0:"
            ls -l ./Aoc2023/bin/Debug/net8.0 || true
            cp -r ./Aoc2023/bin/Debug/net8.0/* $out/
          '';

          checkPhase = ''
            echo "Running tests..."
            dotnet restore Aoc2023.Tests/Aoc2023.Tests.csproj --configfile NuGet.Config
            dotnet build Aoc2023.Tests/Aoc2023.Tests.csproj --configuration Debug
            dotnet test Aoc2023.Tests/Aoc2023.Tests.csproj --no-restore --no-build --verbosity normal
          '';

          doCheck = true;

          buildInputs = [ localNugetRepo ];

          dontFixup = true;
        };
      in
      {
        packages.default = aoc2023Build;

        devShells.default = pkgs.mkShell {
          buildInputs = [
            dotnet
            vscode
            azure-iac-env
            azure-cli
          ];
          shellHook = ''
            echo "Welcome to the Aoc2023 dev shell."
          '';
        };
      }
    );
}
