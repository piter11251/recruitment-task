#!/bin/bash


export GET_TVSHOW_TOTAL_LENGTH_BIN="GetTvShowTotalLength/GetTvShowTotalLength/bin/Debug/net8.0/GetTvShowTotalLength.exe"

if [[ ! -f "$GET_TVSHOW_TOTAL_LENGTH_BIN" ]]; then 
    echo "File not found" >&2
    exit 1
fi 

python3 tv-time/tv-time/tv-time.py