import type { CreateDestinationData } from "../../dtos/CreateDestinationData";
import type { UpdateDestinationRequest } from "../../dtos/UpdateDestinationRequest";
import type { Destination } from "../../models/tripService/Destination";

export interface IDestinationApiService {
  createDestination(data: CreateDestinationData): Promise<Destination>;
  updateDestination(
    data: UpdateDestinationRequest,
    id: string,
  ): Promise<Destination>;
  deleteDestination(id: string): Promise<void>;
}
