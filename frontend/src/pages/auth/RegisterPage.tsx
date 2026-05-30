import { RegisterForm } from "../../components/auth/RegisterForm";

export default function RegisterPage() {
  return (
    <div
      className="min-h-screen w-full flex items-center justify-center relative"
      style={{
        backgroundImage: `url('https://images.unsplash.com/photo-1568430462989-44163eb1752f?w=1920&q=80')`,
        backgroundSize: "cover",
        backgroundPosition: "center",
      }}
    >
      {/* Overlay */}
      <div className="absolute inset-0 bg-gradient-to-b from-blue-950/60 via-blue-900/50 to-blue-950/70" />

      {/* Card */}
      <div className="relative z-10 w-full max-w-md mx-4">
        <div
          className="
          bg-white/10 backdrop-blur-md
          border border-white/20
          rounded-2xl shadow-2xl shadow-blue-950/50
          p-8
        "
        >
          <RegisterForm />
        </div>
      </div>
    </div>
  );
}
