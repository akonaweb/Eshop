import { useEffect, useState } from "react";

export type CategoryType = {
  id: number;
  name: string;
};

type Props = {
  activeCategoryId: number;
  onChange: (id: number) => void;
};

const Categories = ({ activeCategoryId, onChange }: Props) => {
  const [categories, setCategories] = useState<CategoryType[]>([]);

  useEffect(() => {
    const loadCategories = async () => {
      const categoryResult = await (
        await fetch("https://localhost:7203/Category")
      ).json();

      setCategories(categoryResult);
    };

    loadCategories();
  }, []);

  const HandleClicked = (id: number) => {
    onChange(id);
  };

  return (
    <div>
      {categories.map((category) => {
        return (
          <h2
            key={category.id}
            onClick={() => HandleClicked(category.id)}
            style={{
              cursor: "pointer",
              textDecoration:
                category.id === activeCategoryId ? "underline" : "none",
            }}
          >
            {category.name}
          </h2>
        );
      })}
    </div>
  );
};

export default Categories;
