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
  filename=$(basename "$pkg" | tr -d '\r\n')
  echo "[pre-commit] Processing: $filename"

  # Strip the .nupkg extension
  base="${filename%.nupkg}"

  # Extract version (after the last dash)
  version="${base##*-}"
  name="${base%-${version}}"

  # Sanity check
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
