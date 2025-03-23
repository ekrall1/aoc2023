{
  description = "c# environment for aoc";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs =
    {
      self,
      nixpkgs,
      flake-utils,
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

        dotnet = with pkgs; [
          dotnet-sdk
        ];

        azure-iac-env = with pkgs; [
          pulumi-bin
          pulumiPackages.pulumi-language-go
          azure-cli
          go
        ];

      in
      {
        devShells.default = pkgs.mkShell {
          buildInputs = [
            dotnet
            vscode
            azure-iac-env
          ];

          shellHook = ''
            echo "entering C# shell"
          '';
        };
      }

    );
}
