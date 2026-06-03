import { useState } from "react";
import type { TripShareResponse } from "../../../dtos/TripShareResponse";
import { tripShareApiService } from "../../../api_services/tripShare/TripShareApiService";
import { Modal } from "../../ui/Modal";
import { ModalInput } from "../../ui/ModalInput";
import QRCode from "react-qr-code";
import type { ShareTripModalProps } from "../../../props/ShareTripModalProps";

interface FormState {
  email: string;
  accessType: "View" | "Edit";
  expiresInDays: number;
}

interface FormErrors {
  email?: string;
}

export function ShareTripModal({
  isOpen,
  onClose,
  tripId,
}: ShareTripModalProps) {
  const [form, setForm] = useState<FormState>({
    email: "",
    accessType: "View",
    expiresInDays: 7,
  });

  const [errors, setErrors] = useState<FormErrors>({});
  const [shareResult, setShareResult] = useState<TripShareResponse | null>(
    null,
  );
  const [isLoading, setIsLoading] = useState(false);
  const [apiError, setApiError] = useState("");

  const validate = (): boolean => {
    const newErrors: FormErrors = {};
    if (!form.email.trim()) {
      newErrors.email = "Email is required.";
    } else if (!/\S+@\S+\.\S+/.test(form.email)) {
      newErrors.email = "Email is not valid.";
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleCreate = async () => {
    if (!validate()) return;
    setIsLoading(true);
    setApiError("");
    try {
      const result = await tripShareApiService.createShare({
        tripId,
        accessType: form.accessType,
        expiresInDays: form.expiresInDays,
        email: form.email,
      });

      setShareResult(result);
    } catch (err: any) {
      const data = err.response?.data;
      if (typeof data === "string") setApiError(data);
      else if (data?.message) setApiError(data.message);
      else setApiError("Failed to create share link.");
    } finally {
      setIsLoading(false);
    }
  };

  const handleClose = () => {
    setShareResult(null);
    setApiError("");
    setErrors({});
    setForm({ email: "", accessType: "View", expiresInDays: 7 });
    onClose();
  };

  return (
    <Modal isOpen={isOpen} onClose={handleClose} title="Share Trip">
      <div className="flex flex-col gap-4">
        {!shareResult ? (
          <>
            {apiError && (
              <div
                className="px-4 py-3 bg-red-50 border border-red-100
                rounded-xl text-sm text-red-500"
              >
                {apiError}
              </div>
            )}

            <ModalInput
              label="Recipient Email"
              type="email"
              value={form.email}
              onChange={(e) => {
                setForm((prev) => ({ ...prev, email: e.target.value }));
                setErrors((prev) => ({ ...prev, email: "" }));
              }}
              error={errors.email}
              placeholder="friend@email.com"
            />

            <div className="flex flex-col gap-2">
              <label className="text-sm font-medium text-slate-600">
                Access Type
              </label>
              <div className="grid grid-cols-2 gap-2">
                {(["View", "Edit"] as const).map((type) => (
                  <button
                    key={type}
                    onClick={() =>
                      setForm((prev) => ({ ...prev, accessType: type }))
                    }
                    className={`py-3 rounded-xl border text-sm font-medium transition-all
                      ${
                        form.accessType === type
                          ? type === "View"
                            ? "bg-blue-500 border-blue-500 text-white"
                            : "bg-green-500 border-green-500 text-white"
                          : "bg-white border-slate-200 text-slate-600 hover:bg-slate-50"
                      }`}
                  >
                    {type === "View" ? "👁️ View Only" : "✏️ Can Edit"}
                  </button>
                ))}
              </div>
              <p className="text-xs text-slate-400">
                {form.accessType === "View"
                  ? "Recipient can only view the trip plan."
                  : "Recipient can view and edit the trip plan."}
              </p>
            </div>

            <div className="flex flex-col gap-2">
              <label className="text-sm font-medium text-slate-600">
                Expires in{" "}
                <span className="text-blue-500 font-semibold">
                  {form.expiresInDays}{" "}
                  {form.expiresInDays === 1 ? "day" : "days"}
                </span>
              </label>
              <input
                type="range"
                min={1}
                max={30}
                value={form.expiresInDays}
                onChange={(e) =>
                  setForm((prev) => ({
                    ...prev,
                    expiresInDays: Number(e.target.value),
                  }))
                }
                className="w-full accent-blue-500"
              />
              <div className="flex justify-between text-xs text-slate-400">
                <span>1 day</span>
                <span>30 days</span>
              </div>
            </div>

            <div className="flex gap-3 pt-2">
              <button
                onClick={handleClose}
                className="flex-1 py-2.5 rounded-xl border border-slate-200
                  text-slate-600 hover:bg-slate-50 transition-colors text-sm font-medium"
              >
                Cancel
              </button>
              <button
                onClick={handleCreate}
                disabled={isLoading}
                className="flex-1 py-2.5 rounded-xl bg-blue-500 hover:bg-blue-600
                  text-white text-sm font-medium transition-colors
                  disabled:opacity-50 disabled:cursor-not-allowed"
              >
                {isLoading ? "Generating..." : "Generate & Send 📧"}
              </button>
            </div>
          </>
        ) : (
          <div className="flex flex-col items-center gap-4">
            <div className="p-4 bg-white border border-slate-100 rounded-2xl shadow-sm">
              <QRCode value={shareResult.shareUrl} className="w-48 h-48" />
            </div>

            <div className="text-center">
              <p className="text-sm font-medium text-slate-700">
                QR Code sent to{" "}
                <span className="text-blue-500">{form.email}</span>
              </p>
              <span
                className={`inline-block mt-2 px-3 py-1 rounded-full text-xs font-medium
                ${
                  shareResult.accessType === "View"
                    ? "bg-blue-50 text-blue-600"
                    : "bg-green-50 text-green-600"
                }`}
              >
                {shareResult.accessType === "View"
                  ? "👁️ View Only"
                  : "✏️ Can Edit"}
              </span>
              <p className="text-xs text-slate-400 mt-2">
                Expires: {new Date(shareResult.expiresAt).toLocaleDateString()}
              </p>
            </div>

            <button
              onClick={handleClose}
              className="w-full py-2.5 rounded-xl bg-blue-500 hover:bg-blue-600
                text-white text-sm font-medium transition-colors"
            >
              Done
            </button>
          </div>
        )}
      </div>
    </Modal>
  );
}
