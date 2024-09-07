import { useState } from "react"
import logo from "../icons/logo.svg"
import { Link, useNavigate } from "react-router-dom";

export default function Reestablish() {
    const navigate = useNavigate();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [repeatPassword, setRepeatPassword] = useState('');
    const [passwordMatch, setPasswordMatch] = useState('');
    const [successMessage, setSuccessMessage] = useState('');
    const [errorMessage, setErrorMessage] = useState('');

    const handleSubmit = async(event) => {
      event.preventDefault();

      try {
        const response = await fetch('https://localhost:7111/api/auth/reestablish/', {
          method: "POST",
          headers: {
            "Content-Type": "application/json"
          },
          body: JSON.stringify({ email, password}),
        });
  
        if(response.ok) {
          setSuccessMessage(`${await response.text()}`);
          setErrorMessage('');

          setTimeout(() => {navigate('/login')}, 2000);
        }
        else {
          setErrorMessage(`${await response.text()}`);
          setSuccessMessage('');
        }
      } 
      catch(err) {
        alert(err);
      }
    }

    const checkPassword = (newPassword) => {
      setPasswordMatch(newPassword !== password);
    }

  return (
    <div className="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8 sm:max-w-md">
      <div className="bg-white rounded-xl p-10">
        <div className="sm:mx-auto sm:w-full sm:max-w-sm">
          <img
            alt="Your Company"
            src={logo}
            className="mx-auto h-8 w-auto"
          />
        </div>

        <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
          <form onSubmit={handleSubmit} className="space-y-6">

            <div className="">
              {successMessage && (
                <div className="block rounded-md px-3 py-2 ring-1 ring-inset ring-green-600 bg-green-50">
                  <p className="text-center text-green-600">{successMessage}</p>
                </div>
              )}
              {errorMessage && (
                <div className="block rounded-md px-3 py-2 ring-1 ring-inset ring-rose-600 bg-rose-50">
                  <p className="text-center text-rose-600">{errorMessage}</p>
                </div>
              )}
            </div>

            <div>
              <label htmlFor="email" className="block text-sm font-medium leading-6 text-gray-900">
                Эл.почта
              </label>
              <div className="mt-2">
                <input
                  id="email"
                  name="email"
                  // type="email"
                  required
                  autoComplete="email"
                  onChange={(e) => setEmail(e.target.value)}
                  className="block w-full rounded-md px-3 border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-0 focus:ring-inset focus:ring-sky-400 sm:text-sm sm:leading-6"
                />
              </div>
            </div>

            <div>
              <div className="flex items-center justify-between">
                <label htmlFor="password" className="block text-sm font-medium leading-6 text-gray-900">
                  Пароль
                </label>
              </div>
              <div className="mt-2">
                <input
                  id="password"
                  name="password"
                  type="password"
                  required
                  autoComplete="current-password"
                  onChange={(e) => {
                    setPassword(e.target.value);
                    checkPassword(e.target.value);
                  }}
                  className="block w-full rounded-md px-3 border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-0 focus:ring-inset focus:ring-[#2990CE] sm:text-sm sm:leading-6"
                />
              </div>
            </div>

            <div>
              <div className="flex items-center justify-between">
                <label htmlFor="repeatePassword" className="block text-sm font-medium leading-6 text-gray-900">
                  Подтвердите пароль
                </label>
              </div>
              <div className="mt-2">
                <input
                  id="repeatePassword"
                  name="repeatePassword"
                  type="password"
                  required
                  autoComplete="current-password"
                  onChange={(e) => {
                    setRepeatPassword(e.target.value);
                    checkPassword(e.target.value);
                  }}
                  className="block w-full rounded-md px-3 border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-0 focus:ring-inset focus:ring-[#2990CE] sm:text-sm sm:leading-6"
                />
              </div>
            </div>

            <div>
              {passwordMatch && (
                <p className="-mt-2 text-sm font-medium leading-6 text-rose-600">Пароли не совпадают!</p>
              )}
            </div>

            <div>
              <button
                disabled={passwordMatch}
                type="submit"
                className="flex w-full justify-center rounded-md bg-[#2990CE] px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-[#2FA2E7] focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-[#2FA2E7]"
              >
                Изменить пароль
              </button>
            </div>
          </form>

          <p className="mt-10 flex justify-between text-sm text-gray-500">
            <Link to="/login" className="font-semibold leading-6 text-[#2990CE] hover:text-[#2FA2E7]">
              Вход
            </Link>
            <Link to="/register" className="font-semibold leading-6 text-[#2990CE] hover:text-[#2FA2E7]">
              Регистрация
            </Link>
          </p>
        </div>
      </div>
    </div>
  )
}
