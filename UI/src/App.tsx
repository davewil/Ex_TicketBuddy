import {Route, Routes} from "react-router-dom";
import {Home} from "./views/Home.tsx";
import {ToastContainer} from "react-toastify";
import {Header} from "./components/Header.tsx";
import {MainContainer} from "./components/MainContainer.styles.tsx";

function App() {

  return (
      <>
          <Header />
          <MainContainer>
              <AppRoutes />
              <ToastContainer />
          </MainContainer>
      </>
  );
}

export const AppRoutes = () => (
    <Routes>
        <Route path="*" element={<Home/>} />
    </Routes>
);

export default App