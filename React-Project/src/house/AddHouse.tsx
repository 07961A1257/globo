import { useAddHouse } from "../hooks/HouseHooks";
import { House } from "../types/house";
import HouseForm from "./HouseForm";

const AddHouse = () => {
  const addHouseMutation = useAddHouse();
  const newHouse: House = {
    address: "",
    country: "",
    description: "",
    id: 0,
    price: 0,
    photo: "",
  };

  return (
    <HouseForm house={newHouse} submitted={(h) => addHouseMutation.mutate(h)} />
  );
};

export default AddHouse;
