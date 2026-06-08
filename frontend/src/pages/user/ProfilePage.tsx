import { Navbar } from "../../components/layout/Navbar";
import { ModalInput } from "../../components/ui/ModalInput";
import { useProfileForm } from "../../hooks/profil/useProfileForm";

export function ProfilePage() {
  const {
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
  } = useProfileForm();

  if (isLoading) {
    return (
      <div className="min-h-screen bg-slate-50">
        <Navbar />
        <div className="flex items-center justify-center py-20 text-slate-500 text-sm">
          Loading profile...
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-slate-50">
      <Navbar />
      <main className="max-w-2xl mx-auto px-6 py-8">
        <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-6">
          <h1 className="text-xl font-bold text-slate-800 mb-6">
            Profile Settings
          </h1>

          <div className="flex flex-col gap-4">
            <ModalInput
              label="Name"
              type="text"
              value={name}
              onChange={(e) => handleFieldChange("name", e.target.value)}
              error={errors.name}
              placeholder="Your Name"
            />
            <ModalInput
              label="Email"
              type="email"
              value={email}
              onChange={(e) => handleFieldChange("email", e.target.value)}
              error={errors.email}
              placeholder="your.email@example.com"
            />
            <div className="flex flex-col gap-1">
              <label className="text-sm font-medium text-slate-600">Role</label>
              <input
                type="text"
                value={profile?.role ?? ""}
                disabled
                className="px-4 py-2.5 rounded-xl border border-slate-100 text-slate-400 bg-slate-50 cursor-not-allowed text-sm"
              />
              <p className="text-xs text-slate-400">
                Role can only be changed by an administrator.
              </p>
            </div>

            {apiError && (
              <div className="px-4 py-3 bg-red-50 border border-red-100 rounded-xl text-sm text-red-500">
                {apiError}
              </div>
            )}

            {success && (
              <div className="px-4 py-3 bg-green-50 border border-green-100 rounded-xl text-sm text-green-600">
                ✅ Profile updated successfully.
              </div>
            )}
          </div>

          <div className="flex justify-end mt-6 pt-6 border-t border-slate-100">
            <button
              onClick={handleSave}
              disabled={isSaving}
              className="px-6 py-2.5 rounded-xl bg-blue-500 hover:bg-blue-600 text-white font-medium text-sm transition-colors disabled:opacity-50"
            >
              {isSaving ? "Saving..." : "Save Changes"}
            </button>
          </div>
        </div>
      </main>
    </div>
  );
}
