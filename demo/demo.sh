set -e
API=${API:-http://api:8080}
curl -s --retry 30 --retry-delay 1 --retry-connrefused -o /dev/null "$API/"
