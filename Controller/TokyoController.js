const influxService = require('../Service/influx_service');
const pgService = require('../Service/postgres_service');
const influx = influxService.getClient();

class TokyoController {
  
    // GET Weather Data (เลือกจาก is_active = true)
    async getWeatherData(req, res) {
    // const {is_select = false } = req.body; 
    try {
      // ดึงทุกฟิลด์ที่ is_active = TRUE สำหรับ location Tokyo
      const configRows = await pgService.query(
        `SELECT influx_field
         FROM influx_topic_sensor
         WHERE location = $1 AND is_active = TRUE`,
        ['Tokyo']
      );

      if (configRows.length === 0) {
        return res.status(404).json({ error: 'No active fields found for this location' });
      }

      const fieldsToSelect = configRows.map(row => row.influx_field).join(', ');

      // Query จาก InfluxDB โดยไม่ต้องใช้ topic
      const result = await influx.query(`
        SELECT ${fieldsToSelect}
        FROM weather
        ORDER BY time DESC
        LIMIT 50
      `);

      res.json({
        // active_fields: configRows.map(row => row.influx_field),
        data: result
      });

    } catch (err) {
      res.status(500).json({ error: err.message });
    }
  }

  async selectWeatherTko(req, res) {
    const {influx_field} = req.body;

    if (!Array.isArray(influx_field) === 'undefined') {
      return res.status(400).json({ error: 'Missing or invalid parameters' });
    }

    try {

        await pgService.query(
            `CALL proc_influx_fields_set_false($1)`,
            ['Tokyo']
        );


        await pgService.query(
            `CALL proc_influx_fields_selected($1, $2)`,
            ['Tokyo',influx_field]
        );
        const selectRows = await pgService.query(
        'SELECT * FROM fn_get_influx_field($1)',
        ['Tokyo']
      );

      const fieldsToSelect = selectRows.map(row => row.influx_field).join(', ');

      const influxResult = await influx.query(`
        SELECT ${fieldsToSelect}
        FROM weather
        WHERE topic = 'sensor/tokyo'
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

module.exports = new TokyoController();