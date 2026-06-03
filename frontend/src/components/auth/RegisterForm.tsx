import { useState } from "react";
import { useAuth } from "../../hooks/auth/useAuthHook";
import { Link, useLocation, useNavigate } from "react-router-dom";
import type { RegisterRequest } from "../../dtos/RegisterRequest";
import AuthError from "./AuthError";
import FormInput from "./FormInput";

export function RegisterForm() {
  const { register } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const [form, setForm] = useState<RegisterRequest>({
    name: "",
    email: "",
    password: "",
    role: "User",
  });

  const [confirmPassword, setConfirmPassword] = useState("");
  const [errors, setErrors] = useState<
    Partial<RegisterRequest & { confirmPassword: string }>
  >({});
  const [apiError, setApiError] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const validate = (): boolean => {
    const newErrors: Partial<RegisterRequest & { confirmPassword: string }> =
      {};

    if (!form.name) newErrors.name = "Name is required.";

    if (!form.email) {
      newErrors.email = "Email is required.";
    } else if (!/\S+@\S+\.\S+/.test(form.email)) {
      newErrors.email = "Email is not valid.";
    }

    if (!form.password) {
      newErrors.password = "Password is required.";
    } else if (form.password.length < 6) {
      newErrors.password = "Password must be at least 6 characters.";
    }

    if (!confirmPassword) {
      newErrors.confirmPassword = "Please confirm your password.";
    } else if (form.password !== confirmPassword) {
      newErrors.confirmPassword = "Passwords do not match.";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleChange =
    (field: keyof RegisterRequest) =>
    (e: React.ChangeEvent<HTMLInputElement>) => {
      setForm((prev) => ({ ...prev, [field]: e.target.value }));
      setErrors((prev) => ({ ...prev, [field]: "" }));
    };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validate()) return;

    setIsLoading(true);
    setApiError("");

    try {
      await register(form);

      const from = location.state?.from || "/home";
      navigate(from, { replace: true });
    } catch (err: any) {
      const data = err.response?.data;

      if (typeof data === "string") {
        setApiError(data);
      } else if (data?.message) {
        setApiError(data.message);
      } else if (data?.errors) {
        setApiError(Object.values(data.errors).flat().join(", "));
      } else {
        setApiError(err.message || "Login failed.");
      }
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="flex flex-col gap-5 w-full">
      <div className="flex flex-col items-center gap-1 mb-2">
        <h2 className="text-2xl font-bold text-white tracking-tight">
          Create account
        </h2>
        <p className="text-blue-200 text-sm">Start planning your trips today</p>
      </div>

      <AuthError message={apiError} />

      <FormInput
        label="Full Name"
        type="text"
        value={form.name}
        onChange={handleChange("name")}
        error={errors.name}
        placeholder="John Doe"
      />

      <FormInput
        label="Email"
        type="email"
        value={form.email}
        onChange={handleChange("email")}
        error={errors.email}
        placeholder="your@email.com"
      />

      <FormInput
        label="Password"
        type="password"
        value={form.password}
        onChange={handleChange("password")}
        error={errors.password}
        placeholder="••••••••"
      />

      <FormInput
        label="Confirm Password"
        type="password"
        value={confirmPassword}
        onChange={(e) => {
          setConfirmPassword(e.target.value);
          setErrors((prev) => ({ ...prev, confirmPassword: "" }));
        }}
        error={errors.confirmPassword}
        placeholder="••••••••"
      />

      <button
        type="submit"
        disabled={isLoading}
        className="
          w-full py-3 rounded-xl font-semibold text-white tracking-wide
          bg-gradient-to-r from-blue-400/80 to-cyan-400/80
          hover:from-blue-400 hover:to-cyan-400
          border border-white/20
          backdrop-blur-sm
          transition-all duration-200
          disabled:opacity-50 disabled:cursor-not-allowed
          focus:outline-none focus:ring-2 focus:ring-blue-300/50
          shadow-lg shadow-blue-900/30
        "
      >
        {isLoading ? (
          <span className="flex items-center justify-center gap-2">
            <svg
              className="animate-spin h-4 w-4"
              viewBox="0 0 24 24"
              fill="none"
            >
              <circle
                className="opacity-25"
                cx="12"
                cy="12"
                r="10"
                stroke="currentColor"
                strokeWidth="4"
              />
              <path
                className="opacity-75"
                fill="currentColor"
                d="M4 12a8 8 0 018-8v8z"
              />
            </svg>
            Creating account...
          </span>
        ) : (
          "Create account"
        )}
      </button>

      <p className="text-center text-sm text-blue-200">
        Already have an account?{" "}
        <Link
          to="/login"
          className="text-cyan-300 hover:text-white font-medium transition-colors"
        >
          Sign in
        </Link>
      </p>
    </form>
  );
}
