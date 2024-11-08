import os
import subprocess
import sys
from concurrent.futures import ThreadPoolExecutor, as_completed

repo_path = os.path.dirname(os.path.abspath(__file__))
exe_path = os.path.join(repo_path, '..', '..', '..', 'GetTvShowTotalLength', 'GetTvShowTotalLength', 'bin', 'Debug', 'net8.0', 'GetTvShowTotalLength.exe')
os.environ['GET_TVSHOW_TOTAL_LENGTH_BIN'] = exe_path
GET_TVSHOW_TOTAL_LENGTH_BIN = os.getenv('GET_TVSHOW_TOTAL_LENGTH_BIN')

def get_show_length(show_name):
    try:
        result = subprocess.run(
            [GET_TVSHOW_TOTAL_LENGTH_BIN, show_name],
            capture_output=True,
            text=True
        )
        if result.returncode != 0:
            print(f"Could not get info for {show_name}", file=sys.stderr)
            return None


        return (show_name, int(result.stdout.strip()))
    except Exception as e:
        print(e, file=sys.stderr)
        return None

def format_time(minutes):
    hours=minutes//60
    mins = minutes%60
    return f"{hours}h {mins}m"

def main():
    show = [line.strip() for line in sys.stdin if line.strip()]
    if not show:
        sys.exit(1)

    results = []
    with ThreadPoolExecutor() as executor:
        future_to_show = {executor.submit(get_show_length, show): show for show in show}
        for future in as_completed(future_to_show):
            result = future.result()
            if result:
                results.append(result)

    if results:
        shortest_show = min(results, key=lambda x:x[1])
        longest_show = max(results, key=lambda x:x[1])

        print(f"The shortest show: {shortest_show[0]} ({format_time(shortest_show[1])})")
        print(f"The longest show: {longest_show[0]} ({format_time(longest_show[1])})")
    else:
        print("No shows found.")

if __name__ == '__main__':
    main()
