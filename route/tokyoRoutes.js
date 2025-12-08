const express = require('express');
const router = express.Router();
const tokyoController = require('../Controller/TokyoController');

router.get('/weather', tokyoController.getWeatherData);
router.put('/selectWeather', tokyoController.selectWeatherTko);

module.exports = router;