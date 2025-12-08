require('dotenv').config();
const { InfluxDB } = require('influx');

class InfluxService {
  constructor() {
    const requiredEnv = ['INFLUX_HOST', 'INFLUX_PORT', 'INFLUX_DB', 'INFLUX_USER', 'INFLUX_PASS'];
    requiredEnv.forEach((key) => {
      if (!process.env[key]) {
        throw new Error(`Missing env variable: ${key}`);
      }
    });

    this.client = new InfluxDB({
      host: process.env.INFLUX_HOST,
      port: Number(process.env.INFLUX_PORT),
      database: process.env.INFLUX_DB,
      username: process.env.INFLUX_USER,
      password: process.env.INFLUX_PASS,
    });
  }

  getClient() {
    return this.client;
  }
}

module.exports = new InfluxService();