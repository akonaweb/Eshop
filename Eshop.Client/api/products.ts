import { Category } from "./categories";

export type Product = {
  id: number;
  title: string;
  description: string;
  price: number;
  category: Category;
  image: string;
};

export const getProducts = async (categoryId: number): Promise<Product[]> => {
  const result: Product[] = await (
    await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Product`)
  ).json();

  return result
    .filter((x) => x.category.id === categoryId)
    .map((x) => ({ ...x, image: "https://picsum.photos/300" }));
};
