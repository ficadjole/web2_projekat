import { useState, useEffect, useCallback } from "react";
import type { User } from "../../models/user/User";
import { usersApi } from "../../api_services/users/UsersAPIService";

export const useAdminUsers = () => {
  const [users, setUsers] = useState<User[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");

  const fetchUsers = useCallback(async () => {
    try {
      setIsLoading(true);
      const data = await usersApi.getAllUsers();
      setUsers(data);
    } catch (err: any) {
      setError(err.response?.data || "Failed to load users.");
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchUsers();
  }, [fetchUsers]);

  const updateUser = async (id: string, name: string, email: string) => {
    const updated = await usersApi.updateUser(id, { name, email });
    setUsers((prev) => prev.map((u) => (u.id === id ? updated : u)));
  };

  const updateRole = async (id: string, role: "User" | "Admin") => {
    const updated = await usersApi.updateRole(id, { role });
    setUsers((prev) => prev.map((u) => (u.id === id ? updated : u)));
  };

  const deleteUser = async (id: string) => {
    await usersApi.deleteUser(id);
    setUsers((prev) => prev.filter((u) => u.id !== id));
  };

  return {
    users,
    isLoading,
    error,
    updateUser,
    updateRole,
    deleteUser,
  };
};
