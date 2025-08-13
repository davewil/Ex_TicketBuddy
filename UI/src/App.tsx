import {Route, Routes} from "react-router-dom";
import {Home} from "./views/Home.tsx";
import {ToastContainer} from "react-toastify";
import {Header} from "./components/Header.tsx";
import {MainContainer} from "./components/MainContainer.styles.tsx";
import {useUsersStore} from "./stores/users.store.ts";
import {useShallow} from "zustand/react/shallow";
import {useEffect} from "react";
import {EventsManagement} from "./views/EventsManagement.tsx";

function App() {
    const { fetchUsers } = useUsersStore(useShallow((state => ({
        fetchUsers: state.fetchUsers
    }))));

    useEffect(() => {
        fetchUsers();
    }, [fetchUsers]);

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
        <Route path="/events-management" element={<EventsManagement />} />
        <Route path="*" element={<Home/>} />
    </Routes>
);

export default App