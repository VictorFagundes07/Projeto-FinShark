import { Outlet } from "react-router";
import Navbar from "./Components/Navbar/NavBar";
import "./App.css";

function App() {
  return (
    <>
      <Navbar />
      <Outlet />
    </>
  );
}

export default App;