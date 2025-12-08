require('dotenv').config();
const { Pool } = require('pg');

class PostgresService {
  constructor() {
    this.pool = new Pool({
      host: process.env.PG_HOST,
      port: Number(process.env.PG_PORT),
      database: process.env.PG_DB,
      user: process.env.PG_USER,
      password: process.env.PG_PASS,
    });
  }

  async query(sql, params) {
    const client = await this.pool.connect();
    try {
      const res = await client.query(sql, params);
      return res.rows;
    } finally {
      client.release();
    }
  }
}

module.exports = new PostgresService();