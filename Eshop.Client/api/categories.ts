export type Category = {
  id: number;
  name: string;
};

export const getCategories = async (): Promise<Category[]> => {
  const result = await (
    await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Category`)
  ).json();

  return result;
};
