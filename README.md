# Document store

Requires Docker with Compose only.

```
docker compose up --build            # start the service on http://localhost:8080
docker compose run --rm tests        # run the automated tests
docker compose run --rm demo         # run the demo against the service
docker compose down                  # stop; the data volume stays
```
