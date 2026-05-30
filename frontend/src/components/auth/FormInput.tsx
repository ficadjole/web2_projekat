interface FormInputProps {
  label: string;
  type: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  error?: string;
  placeholder?: string;
}

const FormInput = ({
  label,
  type,
  value,
  onChange,
  error,
  placeholder,
}: FormInputProps) => {
  return (
    <div className="flex flex-col gap-1">
      <label className="text-sm font-medium text-blue-100 tracking-wide">
        {label}
      </label>
      <input
        type={type}
        value={value}
        onChange={onChange}
        placeholder={placeholder}
        className={`
          w-full px-4 py-3 rounded-xl text-white placeholder-blue-300
          bg-white/10 border backdrop-blur-sm
          focus:outline-none focus:ring-2 focus:ring-blue-300/50 focus:border-blue-300/50
          transition-all duration-200
          ${
            error
              ? "border-red-400/60 focus:ring-red-400/40"
              : "border-white/20 hover:border-white/30"
          }
        `}
      />
      {error && <span className="text-xs text-red-300 mt-0.5">{error}</span>}
    </div>
  );
};

export default FormInput;
