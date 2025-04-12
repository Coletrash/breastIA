module.exports = {
  database: {
    url: process.env.DB_URL || 'mongodb://localhost:27017/breastia_db',
    options: {
      useNewUrlParser: true,
      useUnifiedTopology: true,
    },
  },
  jwt: {
    secret: process.env.JWT_SECRET || 'breastia_super_secret_key',
    expiresIn: '24h',
  },
  server: {
    port: process.env.PORT || 4000,
  },
};