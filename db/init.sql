CREATE TABLE IF NOT EXISTS urls (
    short_code  TEXT        PRIMARY KEY,
    source_url  TEXT        NOT NULL,
    created_at  TIMESTAMPTZ NOT NULL DEFAULT now()
);