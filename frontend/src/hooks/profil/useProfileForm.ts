import { useState, useEffect } from "react";
import { useAuth } from "../../hooks/auth/useAuthHook";
import { useServices } from "../../contexts/ServiceContext";
import type { User } from "../../models/user/User";

interface FormErrors {
  name?: string;
  email?: string;
}

export function useProfileForm() {
  const { user } = useAuth();
  const { usersApi } = useServices();

  const [profile, setProfile] = useState<User | null>(null);
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);

  const [errors, setErrors] = useState<FormErrors>({});
  const [apiError, setApiError] = useState("");
  const [success, setSuccess] = useState(false);

  useEffect(() => {
    if (!user) return;

    usersApi
      .getUserById(user.id)
      .then((data) => {
        setProfile(data);
        setName(data.name);
        setEmail(data.email);
      })
      .catch((err) => {
        const data = err.response?.data;
        if (typeof data === "string") setApiError(data);
        else setApiError("Failed to load profile.");
      })
      .finally(() => setIsLoading(false));
  }, [user, usersApi]);

  const validate = (): boolean => {
    const newErrors: FormErrors = {};

    if (!name.trim()) {
      newErrors.name = "Name is required.";
    }
    if (!email.trim()) {
      newErrors.email = "Email is required.";
    } else if (!/\S+@\S+\.\S+/.test(email)) {
      newErrors.email = "Invalid email format.";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSave = async () => {
    if (!validate() || !user) return;

    setIsSaving(true);
    setApiError("");
    setSuccess(false);

    try {
      const updatedUser = await usersApi.updateUser(user.id, { name, email });
      setProfile(updatedUser);
      setSuccess(true);
    } catch (err: any) {
      const data = err.response?.data;
      if (typeof data === "string") setApiError(data);
      else setApiError("Failed to update profile.");
    } finally {
      setIsSaving(false);
    }
  };

  const handleFieldChange = (field: "name" | "email", value: string) => {
    if (field === "name") setName(value);
    if (field === "email") setEmail(value);

    if (errors[field]) {
      setErrors((prev) => ({ ...prev, [field]: undefined }));
    }
  };

  return {
    profile,
    name,
    email,
    isLoading,
    isSaving,
    errors,
    apiError,
    success,
    handleFieldChange,
    handleSave,
  };
}
