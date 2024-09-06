import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Register from "./components/Register.jsx";
import Login from "./components/Login.jsx";

function App() {
  return (
    <div className="flex min-h-screen flex-auto justify-center bg-gray-100">
      <Routes>
        <Route path='/login' element={<Login />}/>
        <Route path='/register' element={<Register />}/>
      </Routes>
    </div>
  );
}

export default App;
