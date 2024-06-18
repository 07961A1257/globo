import { useParams } from "react-router-dom";
import { useFetchHouse, useUpdateHouse } from "../hooks/HouseHooks";
import ApiStatus from "../apiStatus";
import HouseForm from "./HouseForm";

const EditHouse = () => {
  const { id } = useParams();
  if (!id) throw Error(`House Id: ${id} doesn't exists`);
  const houseId = parseInt(id);

  const { data, status, isSuccess } = useFetchHouse(houseId);
  const updateHouseMutation = useUpdateHouse();

  if (!isSuccess) return <ApiStatus status={status} />;

  return (
    <HouseForm house={data} submitted={(h) => updateHouseMutation.mutate(h)} />
  );
};

export default EditHouse;
