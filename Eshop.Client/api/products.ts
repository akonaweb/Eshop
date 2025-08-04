import api from "./core/api";
import { Category } from "./categories";
import urls from "./core/urls";

export type Product = {
  id: number;
  title: string;
  description: string;
  price: number;
  category: Category;
  image: string;
};

export const getProducts = async (categoryId: number): Promise<Product[]> => {
  const response = await api().get<Product[]>(urls.product.list);
  return response.data
    .filter((x) => x.category.id === categoryId)
    .map((x) => ({ ...x, image: "https://picsum.photos/300" }));
};
