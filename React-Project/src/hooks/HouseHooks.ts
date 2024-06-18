import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { House } from "../types/house";
import axios, { AxiosError, AxiosResponse } from "axios";
import config from "../config";
import { useNavigate } from "react-router-dom";

const useFetchHouses = () => {
  return useQuery<House[], AxiosError>({
    queryKey: ["houses"],
    queryFn: () =>
      axios.get(`${config.baseApiUrl}/houses`).then((res) => res.data),
  });
};

const useFetchHouse = (id: number) => {
  return useQuery<House, AxiosError>({
    queryKey: ["houses", id],
    queryFn: () =>
      axios.get(`${config.baseApiUrl}/houses/${id}`).then((res) => res.data),
  });
};

const useAddHouse = () => {
  const nav = useNavigate();
  const queryClient = useQueryClient();
  return useMutation<AxiosResponse, AxiosError, House>({
    mutationFn: (h) => axios.post(`${config.baseApiUrl}/houses`, h),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["houses"],
      });
      nav("/");
    },
  });
};

const useUpdateHouse = () => {
  const nav = useNavigate();
  const queryClient = useQueryClient();
  return useMutation<AxiosResponse, AxiosError, House>({
    mutationFn: (h) => axios.put(`${config.baseApiUrl}/houses`, h),
    onSuccess: (_, house) => {
      queryClient.invalidateQueries({
        queryKey: ["houses"],
      });
      nav(`/house/${house.id}`);
    },
  });
};

const useDeleteHouse = () => {
  const nav = useNavigate();
  const queryClient = useQueryClient();
  return useMutation<AxiosResponse, AxiosError, House>({
    mutationFn: (h) => axios.delete(`${config.baseApiUrl}/houses/${h.id}`),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["houses"],
      });
      nav(`/`);
    },
  });
};

export default useFetchHouses;
export { useFetchHouse, useAddHouse, useUpdateHouse, useDeleteHouse };
