#!/bin/bash
set -e

# Adjust Postgres configurations
echo "max_connections = 100" >> $PGDATA/postgresql.conf
echo "shared_buffers = 250MB" >> $PGDATA/postgresql.conf
