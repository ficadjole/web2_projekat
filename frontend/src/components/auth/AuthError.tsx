interface AuthErrorProps {
  message: string;
}

const AuthError = ({ message }: AuthErrorProps) => {
  if (!message) return null;
  return (
    <div className="w-full px-4 py-3 rounded-xl bg-red-500/20 border border-red-400/30 backdrop-blur-sm">
      <p className="text-sm text-red-200 text-center">{message}</p>
    </div>
  );
};

export default AuthError;
