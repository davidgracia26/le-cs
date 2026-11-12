set -e
API=${API:-http://api:8080}
curl -s --retry 30 --retry-delay 1 --retry-connrefused -o /dev/null "$API/"

check() { [ "$2" = "$3" ] || { echo "FAIL $1: expected '$2' got '$3'"; exit 1; }; echo "ok $1"; }
ID=$(cat /proc/sys/kernel/random/uuid)
post() { curl -s -o /dev/null -w "%{http_code}" -X POST -d "$1" "$API/demo/$ID"; }

check "POST first" 200 "$(post '{"n":1}')"
check "POST second" 200 "$(post '{"n":2}')"
check "GET latest" '{"n":2}' "$(curl -s "$API/demo/$ID")"
check "GET versions" '[1,2]' "$(curl -s "$API/demo/$ID/versions")"
check "GET version 1" '{"n":1}' "$(curl -s "$API/demo/$ID/versions/1")"
