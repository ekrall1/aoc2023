#!/usr/bin/env bash
set -euo pipefail

echo "[pre-commit] Restoring NuGet packages into ./nupkgs..."

rm -rf ./nupkgs
mkdir -p ./nupkgs

# Restore all packages into ./nupkgs
dotnet restore --packages ./nupkgs

echo "[pre-commit] Done restoring NuGet packages."

# Optional: generate deps.nix with SHA256s
echo "[pre-commit] Generating deps.nix with SHA256s..."
mkdir -p nuget-deps
deps_file=nuget-deps/deps.nix

echo "[" > "$deps_file"

for pkg in ./nupkgs/*.nupkg; do
  name=$(basename "$pkg" | sed -E 's/-[^-]+\.nupkg$//')
  version=$(basename "$pkg" | sed -E 's/.*-([^-]+)\.nupkg$/\1/')
  sha256=$(nix hash file "$pkg" | sed 's/sha256-//')

  cat >> "$deps_file" <<EOF
{
  name = "${name}";
  version = "${version}";
  sha256 = "sha256-${sha256}";
}
EOF
done

echo "]" >> "$deps_file"

echo "[pre-commit] Wrote SHA-pinned nuget-deps/deps.nix"

