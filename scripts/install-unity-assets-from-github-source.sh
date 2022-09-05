#!/usr/bin/env bash

SCRIPT_DIR="$( cd -- "$( dirname -- "${BASH_SOURCE[0]:-$0}"; )" &> /dev/null && pwd 2> /dev/null; )";
unity_project_root="${SCRIPT_DIR}/../unity/helms-deep-tower-defense"
# data struct = dir name | github URL | branch | source sub folder | destination folder
target_repos=( \
  "NavMeshComponents|https://github.com/Unity-Technologies/NavMeshComponents.git|2020.2|Assets/NavMeshComponents|${unity_project_root}/Assets/NavMeshComponents" \
  "TextMeshPro|https://github.com/Unity-Technologies/NavMeshComponents.git|2020.2|Assets/NavMeshComponents|${unity_project_root}/Assets/NavMeshComponents" \
)

for repo in "${target_repos[@]}" ; do
  IFS='|' read -ra TUPLE_SPLIT <<< "${repo}"
  dir_name="${TUPLE_SPLIT[0]}"
  github_url="${TUPLE_SPLIT[1]}"
  github_tag="${TUPLE_SPLIT[2]}"
  source_dir="${TUPLE_SPLIT[3]}"
  destination_path="${TUPLE_SPLIT[4]}"
  echo "${github_url}"
  echo "${github_tag}"
  pushd /tmp
  rm -rf "${dir_name}"
  git clone --depth 1 --branch "${github_tag}" "${github_url}"
  mkdir -p "${destination_path}"
  rsync -r "${dir_name}"/"${source_dir}"/ "${destination_path}"
  popd
done
