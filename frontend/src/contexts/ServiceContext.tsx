import { createContext, useContext, type ReactNode } from "react";
import { activityApiService } from "../api_services/acitivity/ActivityApiService";
import { authApi } from "../api_services/auth/AuthAPIService";
import { destinationApiService } from "../api_services/destination/DestinationApiService";
import { expenseApiService } from "../api_services/expense/ExpenseApiService";
import { tripApiService } from "../api_services/trip/TripApiService";
import { tripShareApiService } from "../api_services/tripShare/TripShareApiService";
import { usersApi } from "../api_services/users/UsersAPIService";

const services = {
  activityApiService: activityApiService,
  authaPI: authApi,
  destinationApiService: destinationApiService,
  expenseApiService: expenseApiService,
  tripApiService: tripApiService,
  tripShareApiService: tripShareApiService,
  usersApi: usersApi,
};

const ServiceContext = createContext(services);

export function useServices() {
  return useContext(ServiceContext);
}

interface ServiceProviderProps {
  children: ReactNode;
}

export function ServiceProvider({ children }: ServiceProviderProps) {
  return (
    <ServiceContext.Provider value={services}>
      {children}
    </ServiceContext.Provider>
  );
}
