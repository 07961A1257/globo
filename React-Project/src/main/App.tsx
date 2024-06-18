import "./App.css";
import Header from "./Header";
import HouseList from "../house/HouseList";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import HouseDetail from "../house/HouseDetail";
import AddHouse from "../house/AddHouse";
import EditHouse from "../house/EditHouse";

function App() {
  return (
    <BrowserRouter>
      <div className="container">
        <Header subtitle="Providing houses all over the world" />
        <Routes>
          <Route path="/" element={<HouseList />} />
          <Route path="/house/:id" element={<HouseDetail />} />
          <Route path="/house/add" element={<AddHouse />} />
          <Route path="/house/edit/:id" element={<EditHouse />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;
