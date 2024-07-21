import axios from 'axios';
import { jwtDecode } from 'jwt-decode';


const API_URL = 'http://localhost:5001/api/auth';

class AuthService {
  constructor() {
    const token = localStorage.getItem('token');
    const decodedtoken= jwtDecode(token);
    if (token) {
      this.setAuthHeader(token);
      console.log(decodedtoken);
    }
  }

  setAuthHeader(token) {
    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
  }

  async login(user) {
    const response = await axios.post(`${API_URL}/login`, {
      username: user.username,
      password: user.password
    });
    const token = response.data.token;
    localStorage.setItem('token', token);
    this.setAuthHeader(token);
    return response;
  }

  register(user) {
    return axios.post(`${API_URL}/register`, {
      username: user.username,
      email: user.email,
      password: user.password,
      role: user.role // Include role in the request
    });
  }

  logout() {
    localStorage.removeItem('token');
    delete axios.defaults.headers.common['Authorization'];
  }
}

export default new AuthService();
