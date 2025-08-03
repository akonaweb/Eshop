import { getClientAccessToken } from "./accessToken";
import api from "./api";
import { Category } from "./categories";
import urls from "./urls";

export type Product = {
  id: number;
  title: string;
  description: string;
  price: number;
  category: Category;
  image: string;
};

export const getProducts = async (categoryId: number): Promise<Product[]> => {
  const resonse = await api(getClientAccessToken()).get<Product[]>(
    urls.product.list
  );
  return resonse.data
    .filter((x) => x.category.id === categoryId)
    .map((x) => ({ ...x, image: "https://picsum.photos/300" }));
};
