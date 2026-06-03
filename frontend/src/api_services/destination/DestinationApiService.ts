import type { CreateDestinationData } from "../../dtos/CreateDestinationData";
import type { UpdateDestinationRequest } from "../../dtos/UpdateDestinationRequest";
import type { Destination } from "../../models/tripService/Destination";
import api from "../api";
import type { IDestinationApiService } from "./IDestinationApiService";

export const destinationApiService: IDestinationApiService = {
  createDestination: async (
    data: CreateDestinationData,
  ): Promise<Destination> => {
    const response = await api.post<Destination>(
      `/api/Destination/create`,
      data,
    );

    return response.data;
  },
  updateDestination: async (
    data: UpdateDestinationRequest,
    id: string,
  ): Promise<Destination> => {
    const response = await api.put<Destination>(`/api/Destination/${id}`, data);

    return response.data;
  },
  deleteDestination: async (id: string): Promise<void> => {
    await api.delete(`/api/Destination/${id}`);
  },
};
