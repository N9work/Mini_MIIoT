const express = require('express');
const router = express.Router();
const bangkokController = require('../Controller/BangkokController');

router.get('/weather', bangkokController.getWeatherData);
router.put('/selectWeather', bangkokController.selectWeatherBkk);

module.exports = router;