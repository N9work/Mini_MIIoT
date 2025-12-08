const influxService = require('../Service/influx_service');
const pgService = require('../Service/postgres_service');

const influx = influxService.getClient();

class BangkokController {

async getWeatherData(req, res) {
    try {
  
      const configRows = await pgService.query(
        `SELECT influx_field
         FROM influx_topic_sensor
         WHERE location = $1 AND is_active = TRUE`,
        ['Bangkok']
      );

      if (configRows.length === 0) {
        return res.status(404).json({ error: 'No active fields found for this location' });
      }

      const fieldsToSelect = configRows.map(row => row.influx_field).join(', ');


      const result = await influx.query(`
        SELECT ${fieldsToSelect}
        FROM weather
        ORDER BY time DESC
        LIMIT 50
      `);

      res.json({
        data: result
      });

    } catch (err) {
      res.status(500).json({ error: err.message });
    }
  }

  async selectWeatherBkk(req, res) {
    const { influx_field } = req.body;

    if (!Array.isArray(influx_field)=== 'undefined') {
      return res.status(400).json({ error: 'Missing or invalid parameters' });
    }

    try {

        await pgService.query(
            `UPDATE influx_topic_sensor
             SET is_select = FALSE
             WHERE location = $1`,
            ['Bangkok']
        );

        await pgService.query(
            `UPDATE influx_topic_sensor
             SET is_select = TRUE
             WHERE location = $1 
             AND influx_field = ANY($2::text[])`,
            ['Bangkok',influx_field]
        );


      const selectRows = await pgService.query(
        `SELECT influx_field
         FROM influx_topic_sensor
         WHERE location = $1 AND is_select = TRUE`,
        ['Bangkok']
      );

      const fieldsToSelect = selectRows.map(row => row.influx_field).join(', ');

      const influxResult = await influx.query(`
        SELECT ${fieldsToSelect}
        FROM weather
        WHERE topic = 'sensor/bangkok'
        ORDER BY time DESC
        LIMIT 50
      `);

      res.json({
        // message: 'is_select updated successfully',
        // selected_fields: selectRows.map(row => row.influx_field),
        data: influxResult
      });
    } catch (err) {
      res.status(500).json({ error: err.message });
    }
  }
}

module.exports = new BangkokController();