#!/usr/bin/env bash
set -euo pipefail

echo "[pre-commit] Restoring NuGet packages into ./nupkgs..."

# Clean up previous local packages
rm -rf ./nupkgs
mkdir -p ./nupkgs

# Restore each project explicitly
found_any=false
for proj in Aoc2023/*.csproj Aoc2023.Tests/*.csproj; do
  if [[ -f "$proj" ]]; then
    echo "[pre-commit] Restoring $proj"
    dotnet restore "$proj" --packages ./nupkgs
    found_any=true
  fi
done

if ! $found_any; then
  echo "[pre-commit] No .csproj files found. Aborting."
  exit 1
fi

echo "[pre-commit] Finished restoring packages."

# Generate deps.nix with SHA256 hashes
echo "[pre-commit] Generating nuget-deps/deps.nix with sha256s..."

mkdir -p nuget-deps
deps_file=nuget-deps/deps.nix
tmp_file=$(mktemp)

echo "[" > "$tmp_file"

# Find and hash each unique .nupkg file
find ./nupkgs -name '*.nupkg' -type f | sort | uniq | while read -r pkg; do
  filename=$(basename "$pkg")
  name=$(echo "$filename" | sed -E 's/-[0-9][^-]*\.nupkg$//')
  version=$(echo "$filename" | sed -E 's/.*-([0-9][^-]*)\.nupkg$/\1/')

  # skip if version or name failed to parse
  if [[ -z "$name" || -z "$version" ]]; then
    echo "[pre-commit] Skipping unrecognized package name: $filename"
    continue
  fi

  sha256=$(nix hash file "$pkg" | sed 's/sha256-//')

  cat >> "$tmp_file" <<EOF
{
  name = "${name}";
  version = "${version}";
  sha256 = "sha256-${sha256}";
}
EOF
done

echo "]" >> "$tmp_file"
mv "$tmp_file" "$deps_file"

echo "[pre-commit] Wrote: $deps_file"
