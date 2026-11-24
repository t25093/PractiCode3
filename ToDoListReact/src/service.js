import axios from 'axios';

// כתובת השרת מגיעה ממשתנה סביבה (.env לוקלית, Environment ב-Render)
axios.defaults.baseURL = process.env.REACT_APP_API_URL;

// רישום שגיאות מהשרת לקונסול
axios.interceptors.response.use(
  response => response,
  error => {
    console.error('API response error:', error);
    return Promise.reject(error);
  }
);

// נתיבי השרת (Program.cs):
// GET    /items        -> כל המשימות
// POST   /items        -> הוספת משימה
// PUT    /items/{id}   -> עדכון משימה
// DELETE /items/{id}   -> מחיקת משימה

export default {
  getTasks: async () => {
    const resp = await axios.get('/items');
    return resp.data;
  },

  addTask: async (name) => {
    const resp = await axios.post('/items', { name: name, isComplete: false });
    return resp.data;
  },

  setCompleted: async (id, isComplete) => {
    const resp = await axios.put(`/items/${id}`, { isComplete: isComplete });
    return resp.data;
  },

  deleteTask: async (id) => {
    await axios.delete(`/items/${id}`);
  }
};
