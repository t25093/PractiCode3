require('dotenv').config();              // טוען משתנים מקובץ .env (לוקלית בלבד)
const express = require('express');

const app = express();
const PORT = process.env.PORT || 3000;   // Render קובע את PORT; לוקלית 3000
const RENDER_API_KEY = process.env.RENDER_API_KEY;

// בדיקה שהשרת חי
app.get('/', (req, res) => {
  res.send('Server is running. Go to /services');
});

// רשימת השירותים בחשבון Render
app.get('/services', async (req, res) => {
  try {
    const response = await fetch('https://api.render.com/v1/services?limit=20', {
      method: 'GET',
      headers: {
        accept: 'application/json',
        authorization: `Bearer ${RENDER_API_KEY}`
      }
    });

    if (!response.ok) {
      return res.status(response.status).json({ error: 'Failed to fetch services from Render' });
    }

    const data = await response.json();
    res.json(data);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

app.listen(PORT, () => {
  console.log(`Server is listening on port ${PORT}`);
});
