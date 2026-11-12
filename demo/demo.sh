set -e
API=${API:-http://api:8080}
curl -s --retry 30 --retry-delay 1 --retry-connrefused -o /dev/null "$API/"

check() { [ "$2" = "$3" ] || { echo "FAIL $1: expected '$2' got '$3'"; exit 1; }; echo "ok $1"; }
put() { curl -s -o /dev/null -w "%{http_code}" -X PUT -d "$1" "$API/demo/1"; }

check "PUT first" 200 "$(put '{"n":1}')"
check "PUT second" 200 "$(put '{"n":2}')"
check "GET latest" '{"n":2}' "$(curl -s "$API/demo/1")"
check "GET versions" '[1,2]' "$(curl -s "$API/demo/1/versions")"
check "GET version 1" '{"n":1}' "$(curl -s "$API/demo/1/versions/1")"
