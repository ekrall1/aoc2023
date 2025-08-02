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

        localNugetRepo = pkgs.stdenv.mkDerivation {
          pname = "local-nuget-repo";
          version = "1.0";
          src = ./nupkgs;

          buildPhase = ''
            mkdir -p $out
            cp -r $src/* $out/
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

          buildInputs = [ localNugetRepo ];

          buildPhase = ''
                # Write NuGet.Config to add local package source
                cat > NuGet.Config <<EOF
            <?xml version="1.0" encoding="utf-8"?>
            <configuration>
              <packageSources>
                <add key="local" value="file://${localNugetRepo}" />
              </packageSources>
            </configuration>
            EOF

                export DOTNET_NUGET_CONFIG_FILE=$PWD/NuGet.Config

                dotnet restore ./Aoc2023/Aoc2023.csproj --configfile NuGet.Config
                dotnet build ./Aoc2023/Aoc2023.csproj --configuration Debug

                # Build test project
                dotnet restore ./Aoc2023.Tests/Aoc2023.Tests.csproj --configfile NuGet.Config
                dotnet build ./Aoc2023.Tests/Aoc2023.Tests.csproj --configuration Debug
          '';

          installPhase = ''
            mkdir -p $out
            cp -r ./Aoc2023/bin/Debug/net8.0/* $out/
          '';

          checkPhase = ''
            echo "Running tests..."
            dotnet test ./Aoc2023.Tests/Aoc2023.Tests.csproj --no-build --no-restore --verbosity normal
          '';

          doCheck = true;

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
            nuget-to-nix.packages.${system}.default
          ];
          shellHook = ''
            echo "Welcome to the Aoc2023 dev shell."
            if [ -f .hooks/pre-commit ]; then
            mkdir -p .git/hooks
            cp .hooks/pre-commit .git/hooks/pre-commit
            chmod +x .git/hooks/pre-commit
            echo "[devShell] Installed pre-commit hook."
            fi
          '';
        };
      }
    );
}
