import './App.css'
import {Route, Routes} from "react-router-dom";
import {Home} from "./views/Home.tsx";
import {ToastContainer} from "react-toastify";

function App() {

  return (
      <div>
        <AppRoutes />
        <ToastContainer />
      </div>
  )
}

export const AppRoutes = () => (
    <Routes>
        <Route path="*" element={<Home/>} />
    </Routes>
);

export default App