const express = require('express');
const app = express();
const bangkokRoutes = require('./route/bangkokRoutes');
const tokyoRoutes = require('./route/tokyoRoutes');

// Middleware
app.use(express.json());

// Routes
app.use('/bangkok', bangkokRoutes); // GET /bangkok/weather
app.use('/tokyo', tokyoRoutes);     // GET /tokyo/weather

module.exports = app;