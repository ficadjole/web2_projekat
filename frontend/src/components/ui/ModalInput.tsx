interface ModalInputProps {
  label: string;
  type?: string;
  value: string;
  onChange: (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => void;
  error?: string;
  placeholder?: string;
  textarea?: boolean;
}

export function ModalInput({
  label,
  type = "text",
  value,
  onChange,
  error,
  placeholder,
  textarea,
}: ModalInputProps) {
  const baseClass = `
    w-full px-4 py-2.5 rounded-xl border text-slate-800 text-sm
    focus:outline-none focus:ring-2 focus:ring-blue-300 focus:border-blue-300
    transition-all duration-150 placeholder-slate-300
    ${error ? "border-red-300 focus:ring-red-200" : "border-slate-200"}
  `;

  return (
    <div className="flex flex-col gap-1">
      <label className="text-sm font-medium text-slate-600">{label}</label>
      {textarea ? (
        <textarea
          value={value}
          onChange={onChange}
          placeholder={placeholder}
          rows={3}
          className={`${baseClass} resize-none`}
        />
      ) : (
        <input
          type={type}
          value={value}
          onChange={onChange}
          placeholder={placeholder}
          className={baseClass}
        />
      )}
      {error && <span className="text-xs text-red-500">{error}</span>}
    </div>
  );
}
