import { useEffect, useState } from "react";
import { checklistApiService } from "../../api_services/checklist/ChecklistApiService";
import type { ChecklistItem } from "../../models/checklistService/ChecklistItem";
import type { ChecklistDto } from "../../dtos/ChecklistDto";

export function useChecklist(tripId: string) {
  const [checklist, setChecklist] = useState<ChecklistDto | null>(null);
  const [newItemName, setNewItemName] = useState("");
  const [isAdding, setIsAdding] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    checklistApiService
      .getByTripId(tripId)
      .then(setChecklist)
      .catch(() => setChecklist({ id: "", tripId, items: [] }))
      .finally(() => setIsLoading(false));
  }, [tripId]);

  const handleAddItem = async () => {
    if (!newItemName.trim()) return;
    setIsAdding(true);
    try {
      const item = await checklistApiService.addItem({
        name: newItemName,
        tripId,
      });
      setChecklist((prev) =>
        prev
          ? { ...prev, items: [...prev.items, item] }
          : { id: "", tripId, items: [item] },
      );
      setNewItemName("");
    } finally {
      setIsAdding(false);
    }
  };

  const handleToggle = async (item: ChecklistItem) => {
    const updated = await checklistApiService.toggleItem(tripId, item.id);
    setChecklist((prev) =>
      prev
        ? {
            ...prev,
            items: prev.items.map((i) => (i.id === item.id ? updated : i)),
          }
        : prev,
    );
  };

  const handleDelete = async (itemId: string) => {
    await checklistApiService.deleteItem(tripId, itemId);
    setChecklist((prev) =>
      prev
        ? { ...prev, items: prev.items.filter((i) => i.id !== itemId) }
        : prev,
    );
  };

  const items = checklist?.items ?? [];

  const totalCount = items.length;

  const completedCount = items.filter((i) => i.isChecked).length;

  const completionPercentage =
    totalCount > 0 ? Math.round((completedCount / totalCount) * 100) : 0;

  return {
    items,
    newItemName,
    setNewItemName,
    isAdding,
    isLoading,
    totalCount,
    completedCount,
    completionPercentage,
    handleAddItem,
    handleToggle,
    handleDelete,
  };
}
