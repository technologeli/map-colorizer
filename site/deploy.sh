#!/bin/bash
set -e

npm run build

cd dist

git init
git checkout -b main
git add -A
git commit -m "deploy"

git push -f git@github.com:technologeli/map-colorizer main:gh-pages
